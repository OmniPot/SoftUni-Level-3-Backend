<?php

$configurations['default']['connection_uri'] = 'mysql:host=localhost;dbname=test';
$configurations['default']['username'] = 'root';
$configurations['default']['password'] = 'sera';
$configurations['default']['pdo_options'][PDO::MYSQL_ATTR_INIT_COMMAND] = "SET NAMES 'UTF8'";
$configurations['default']['pdo_options'][PDO::ATTR_ERRMODE] = PDO::ERRMODE_EXCEPTION;

$configurations['session']['connection_uri'] = 'mysql:host=localhost;dbname=session';
$configurations['session']['username'] = 'root';
$configurations['session']['password'] = 'sera';
$configurations['session']['pdo_options'][PDO::MYSQL_ATTR_INIT_COMMAND] = "SET NAMES 'UTF8'";
$configurations['session']['pdo_options'][PDO::ATTR_ERRMODE] = PDO::ERRMODE_EXCEPTION;

return $configurations;