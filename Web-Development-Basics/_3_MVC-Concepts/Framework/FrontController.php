<?php

namespace TODO\Framework;

use TODO\Application\Config\Config;
use TODO\Application\Controllers\BaseController;

class FrontController {

    private static $_instance = null;

    private $controllerName;
    /**
     * @var $controller BaseController
     */
    private $controller;
    /**
     * @var $controllers array
     */
    private $controllers = array();

    private function __construct() {

    }

    public function dispatch() {
        $controllerName = Config::DEFAULT_CONTROLLER;
        $actionName = Config::DEFAULT_ACTION;
        $requestParams = [ ];

        if ( $_GET && isset( $_GET[ 'uri' ] ) ) {
            $uri = trim( $_GET[ 'uri' ] );
        } else {
            header( 'Location: ' . $controllerName . '/' . $actionName );
            exit;
        }

        $uri = explode( '/', rtrim( $_GET[ 'uri' ], '/' ) );

        if ( count( $uri ) >= 2 ) {
            $controllerName = rtrim( array_shift( $uri ) );
            $actionName = count( $uri ) ? rtrim( array_shift( $uri ) ) : Config::DEFAULT_ACTION;
            $requestParams = $uri;
        }

        $fullControllerName =
            Config::VENDOR_NAMESPACE
            . Config::APPLICATION_NAMESPACE
            . Config::CONTROLLERS_NAMESPACE
            . ucfirst( $controllerName )
            . Config::CONTROLLERS_SUFFIX;

        try {
            $this->registerControllers();
            $this->registerActions();

            $this->validateController( $fullControllerName, $controllerName );
            $this->validateAction( $fullControllerName, $actionName );

            $this->initController($controllerName, $fullControllerName, $requestParams );

            View::$controllerName = $controllerName;
            View::$actionName = $actionName;

            call_user_func_array( [ $this->controller, $actionName ], $requestParams );
        } catch ( \Exception $exception ) {
            echo $exception->getMessage();
        }
    }

    private function registerControllers() {
        foreach ( glob( Config::CONTROLLERS_FOLDER ) as $filePath ) {
            if ( file_exists( $filePath ) && is_readable( $filePath ) ) {
                $fileFullPath = Config::VENDOR_NAMESPACE . str_replace( [ '/', '\\' ], DIRECTORY_SEPARATOR, $filePath );
                $fileFullPath = substr( $fileFullPath, 0, -4 );
                $this->controllers[ $fileFullPath ] = true;
            } else {
                throw new \Exception( 'File not found: ' . $filePath );
            }
        }
    }

    private function registerActions() {
        foreach ( $this->controllers as $cKey => $cValue ) {
            $methods = array_values( get_class_methods( $cKey ) );
            $this->controllers[ $cKey ] = array();
            foreach ( $methods as $method ) {
                $this->controllers[ $cKey ][ $method ] = true;
            }
        }
    }

    private function initController($simpleName, $fullQualifiedName, array $requestParams ) {
        if ( isset( $this->controllers[ $fullQualifiedName ] ) ) {
            $this->controller = new $fullQualifiedName( $simpleName, $requestParams );
        } else {
            throw new \Exception( 'File not found: ' . $fullQualifiedName );
        }
    }

    private function validateController( $fullControllerName, $controllerName ) {
        if ( !isset( $this->controllers[ $fullControllerName ] ) ) {
            throw new \Exception( 'Controller: ' . $controllerName . ' not found.' );
        }
    }

    private function validateAction( $fullControllerName, $actionName ) {
        if ( !isset( $this->controllers[ $fullControllerName ][ $actionName ] ) ) {
            throw new \Exception(
                'Controller: ' . $fullControllerName . '
                    contains no method: ' . $actionName );
        }
    }

    /**
     * @return FrontController
     */
    public static function getInstance() {
        if ( self::$_instance == null ) {
            self::$_instance = new self();
        }

        return self::$_instance;
    }
}