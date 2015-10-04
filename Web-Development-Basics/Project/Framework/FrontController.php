<?php

namespace Framework;

use \Framework\Routers\IRouter;

class FrontController {
    private static $_instance = null;
    private $_namespace = null;
    private $_method = null;
    /**
     * @var \Framework\Routers\IRouter
     */
    private $_router = null;
    private $_controller = null;

    private function __construct() {

    }

    public function getRouter() {
        return $this->_router;
    }

    public function setRouter(IRouter $router) {
        $this->_router = $router;
    }

    public function dispatch() {
        if ($this->_router == null) {
            throw new \Exception('No valid router found.', 500);
        }

        $uri = $this->_router->getURI();
        $routes = App::getInstance()->getConfig()->routes;
        $_rc = null;

        if (is_array($routes) && count($routes) > 0) {
            foreach ($routes as $route => $value) {
                if (stripos($uri, $route) === 0 &&
                    ($uri == $route || strpos($uri, $route . '/') === 0) &&
                    $value['namespace']
                ) {
                    $this->_namespace = $value['namespace'];
                    $uri = substr($uri, strlen($route) + 1);
                    $_rc = $value;
                    break;
                }
            }
        } else {
            throw new \Exception('Default route missing', 500);
        }

        if ($this->_namespace == null && $routes['*']['namespace']) {
            $this->_namespace = $routes['*']['namespace'];
            $_rc = $routes['*'];
        } else if ($this->_namespace == null && !$routes['*']['namespace']) {
            throw new \Exception('Default route missing', 500);
        }

        $input = InputData::getInstance();
        $params = explode('/', $uri);
        if ($params[0]) {
            $this->_controller = strtolower($params[0]);
            if ($params[1]) {
                $this->_method = strtolower($params[1]);
                unset($params[0], $params[1]);
                $input->setGet(array_values($params));
            } else {
                $this->_method = $this->getDefaultMethod();
            }
        } else {
            $this->_controller = $this->getDefaultController();
            $this->_method = $this->getDefaultMethod();
        }

        if (is_array($_rc) && $_rc['controllers']) {
            if ($_rc['controllers'][$this->_controller]['methods'][$this->_method]) {
                $this->_method = strtolower($_rc['controllers'][$this->_controller]['methods'][$this->_method]);
            }

            if (isset($_rc['controllers'][$this->_controller]['to'])) {
                $this->_controller = strtolower($_rc['controllers'][$this->_controller]['to']);
            }
        }


        $input->setPost($this->_router->getPost());

        $contructed = $this->_namespace . '\\' . ucfirst($this->_controller);

        $newController = new $contructed();
        $newController->{$this->_method}();
    }

    public function getDefaultController() {
        $controller = App::getInstance()->getConfig()->app['default_controller'];
        if ($controller) {
            return strtolower($controller);
        }

        return 'index';
    }

    public function getDefaultMethod() {
        $method = App::getInstance()->getConfig()->app['default_method'];
        if ($method) {
            return strtolower($method);
        }

        return 'index';
    }

    /**
     * @return \Framework\FrontController
     */
    public static function getInstance() {
        if (self::$_instance == null) {
            self::$_instance = new FrontController();
        }

        return self::$_instance;
    }
}