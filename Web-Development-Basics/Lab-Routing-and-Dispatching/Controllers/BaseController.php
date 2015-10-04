<?php

namespace Softuni\Controllers;

use Softuni\Config\DatabaseConfig;
use Softuni\Core\Database;

class BaseController {

    protected $databaseInstance;
    protected $requestParams;

    public function __construct(array $requestParams = []) {
        $this->requestParams = $requestParams;
        $this->databaseInstance = Database::getInstance(DatabaseConfig::DB_INSTANCE_NAME);
    }

    protected function isLogged() {
        return isset($_SESSION['id']);
    }
}