<?php

use TODO\Framework\App;

session_start();

require_once 'Framework\App.php';

$app = App::getInstance();

$app->start();