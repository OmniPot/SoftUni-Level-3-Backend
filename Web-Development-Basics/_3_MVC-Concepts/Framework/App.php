<?php

namespace TODO\Framework;

use TODO\Application\Config\Config;

class App {

    private static $_instance = null;
    private $_frontController = null;

    public function __construct() {

    }

    public function start() {

        $this->initAutoload();

        Database::setInstance(
            Config::DB_INSTANCE_NAME,
            Config::DB_DRIVER,
            Config::DB_USERNAME,
            Config::DB_PASSWORD,
            Config::DB_NAME,
            Config::DB_HOST
        );

        $this->_frontController = FrontController::getInstance();
        $this->_frontController->dispatch();
    }

    private function initAutoload() {
        spl_autoload_register( function ( $class ) {
            $classPath = str_replace( [ '\\', '/' ], DIRECTORY_SEPARATOR, $class );
            $classPath = str_replace( 'TODO\\', '', $class . '.php' );

            if ( file_exists( $classPath ) && is_readable( $classPath ) ) {
                require_once $classPath;
            } else {
                throw new \Exception( 'File not found or is not readable: ' . $classPath );
            }
        } );
    }

    public static function getInstance() {
        if ( self::$_instance == null ) {
            self::$_instance = new self();
        }

        return self::$_instance;
    }
}