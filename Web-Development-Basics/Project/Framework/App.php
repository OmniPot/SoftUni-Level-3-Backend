<?php

namespace Framework;

use Framework\Routers\IRouter;
use Framework\Routers\DefaultRouter;

use Framework\Sessions\DatabaseSession;
use Framework\Sessions\ISession;
use Framework\Sessions\NativeSession;

use Framework\Utilities;
use Framework\View;

include_once 'Loader.php';

class App {
    private static $_instance = null;
    private $_config = null;
    private $_router = null;
    private $_databaseConnections = array();
    /**
     * @var \Framework\Sessions\ISession
     */
    private $_session = null;

    /**
     * @var \Framework\FrontController
     */
    private $_frontControlzler = null;

    private function __construct() {
        set_exception_handler(array($this, '_exceptionHandler'));
        Loader::registerNamespace('Framework', dirname(__FILE__) . DIRECTORY_SEPARATOR);
        Loader::registerAutoLoad();

        $this->_config = Config::getInstance();

        if ($this->_config->getConfigFolder() == null) {
            $this->setConfigFolder('../config');
        }
    }

    public function getRouter() {
        return $this->_router;
    }

    public function setRouter($router) {
        $this->_router = $router;
    }

    public function setConfigFolder($path) {
        $this->_config->setConfigFolder($path);
    }

    public function getConfigFolder() {
        return $this->_configFolder;
    }

    public function setSession(ISession $session) {
        $this->_session = $session;
    }

    /**
     * @return \Framework\Sessions\ISession
     */
    public function getSession() {
        return $this->_session;
    }

    /**
     * @return \Framework\Config
     */
    public function getConfig() {
        return $this->_config;
    }

    public function run() {
        if ($this->_config->getConfigFolder() == null) {
            $this->setConfigFolder('../config');
        }

        $this->_frontController = FrontController::getInstance();
        if ($this->_router instanceof IRouter) {
            $this->_frontController->setRouter($this->_router);
        } else if ($this->_router == 'JsonRPCRouter') {
            $this->_frontController->setRouter(new DefaultRouter());
        } else if ($this->_router == 'CLIRouter') {
            $this->_frontController->setRouter(new DefaultRouter());
        } else {
            $this->_frontController->setRouter(new DefaultRouter());
        }

        $sessionConfig = $this->_config->app['session'];
        if ($sessionConfig['autostart']) {
            if ($sessionConfig['type'] == 'native') {
                $usableSession = new NativeSession(
                    $sessionConfig['name'],
                    $sessionConfig['lifetime'],
                    $sessionConfig['path'],
                    $sessionConfig['domain'],
                    $sessionConfig['secure']
                );
            } else if ($sessionConfig['type'] == 'database') {
                $usableSession = new DatabaseSession(
                    $sessionConfig['databaseConnection'],
                    $sessionConfig['name'],
                    $sessionConfig['databaseTable'],
                    $sessionConfig['lifetime'],
                    $sessionConfig['path'],
                    $sessionConfig['domain'],
                    $sessionConfig['secure']
                );
            } else {
                throw new \Exception('No valid sessionConfig', 500);
            }

            $this->setSession($usableSession);
        }

        $this->_frontController->dispatch();
    }

    public function getDatabaseConnection($connection = 'default') {
        if (!$connection) {
            throw new \Exception('No connection identifier provided', 500);
        }
        if ($this->_databaseConnections[$connection]) {
            return $this->_databaseConnections[$connection];
        }

        $configurations = $this->getConfig()->database;
        if (!$configurations[$connection]) {
            throw new \Exception('No valid connection identifier is provided', 500);
        }

        $existingConnection = new \PDO($configurations[$connection]['connection_uri'], $configurations[$connection]['username'],
            $configurations[$connection]['password'], $configurations[$connection]['pdo_options']);

        return $existingConnection;
    }

    public function __destruct() {
        if ($this->_session != null) {
            $this->_session->saveSession();
        }
    }

    public function _exceptionHandler(\Exception $exception) {
        if ($this->_config && $this->_config->app['displayExceptions'] == true) {
            echo '<pre>' . print_r($exception, true) . '</pre>';
        } else {
            $this->displayError($exception->getCode());
        }
    }

    public function displayError($error) {
        try {
            $view = View::getInstance();
            $view->display('errors' . $error);
        } catch (\Exception $exception) {
            Utilities::headerStatus($error);
            echo '<h1>' . $error . '</h1>';
            exit;
        }
    }

    /**
     * @return \Framework\App
     */
    public static function getInstance() {
        if (self::$_instance == null) {
            self::$_instance = new App();
        }

        return self::$_instance;
    }
}