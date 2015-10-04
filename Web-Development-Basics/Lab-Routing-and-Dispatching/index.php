<?php

use \Softuni\Core\Database;
use \Softuni\Config\DatabaseConfig;
use \Softuni\App;
use \Softuni\Autoloader;

session_start();

require_once 'Autoloader.php';

Autoloader::init();

if (isset($_GET['uri'])) {

    $uri = explode('/', rtrim($_GET['uri'], '/'));

    if (count($uri)) {
        $controllerName = array_shift($uri);
    }
    if (count($uri)) {
        $actionName = array_shift($uri);
    }

    $params = $uri;
}

Database::setInstance(
    DatabaseConfig::DB_INSTANCE_NAME,
    DatabaseConfig::DB_DRIVER,
    DatabaseConfig::DB_USERNAME,
    DatabaseConfig::DB_PASSWORD,
    DatabaseConfig::DB_NAME,
    DatabaseConfig::DB_HOST
);

$app = new App($controllerName, $actionName, $params);
$app->start();