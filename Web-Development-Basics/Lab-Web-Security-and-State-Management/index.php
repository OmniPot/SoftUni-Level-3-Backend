<?php

use \Core\Database;
use \Config\DatabaseConfig;
use \Core\App;

session_start();

spl_autoload_register(function ($class) {
    $classPath = str_replace('\\', '/', $class . '.php');
    require_once $classPath;
});

require_once 'core/App.php';

Database::setInstance(
    DatabaseConfig::DB_INSTANCE_NAME,
    DatabaseConfig::DB_DRIVER,
    DatabaseConfig::DB_USERNAME,
    DatabaseConfig::DB_PASSWORD,
    DatabaseConfig::DB_NAME,
    DatabaseConfig::DB_HOST
);

$app = new App(Database::getInstance(DatabaseConfig::DB_INSTANCE_NAME));

function loadTemplate($templateName, $data = null){
    require_once 'templates/' . $templateName . '.php';
}