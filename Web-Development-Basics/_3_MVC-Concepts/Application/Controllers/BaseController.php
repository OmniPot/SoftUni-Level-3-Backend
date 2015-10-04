<?php

namespace TODO\Application\Controllers;

use TODO\Application\Config\Config;
use TODO\Framework\Database;

class BaseController {

    protected $controllerName;
    protected $requestParams;

    protected $databaseInstance;
    protected $unauthorizedLocation = 'users/login';
    protected $alreadyAuthorizedLocation = 'todos/all';

    public function __construct( $controllerName, array $requestParams = [ ] ) {
        $this->controllerName = $controllerName;
        $this->requestParams = $requestParams;

        $this->databaseInstance = Database::getInstance( Config::DB_INSTANCE_NAME );
    }

    protected function isLogged() {
        return isset( $_SESSION[ 'id' ] );
    }

    protected function redirect($location) {
        $controllerPos = stripos( $_SERVER[ 'REQUEST_URI' ], $this->controllerName );
        $resultUri = substr( $_SERVER[ 'REQUEST_URI' ], 0, $controllerPos ) . $location;
        header( 'Location: ' . $resultUri );
        die( 'Unauthorized' );
    }
}