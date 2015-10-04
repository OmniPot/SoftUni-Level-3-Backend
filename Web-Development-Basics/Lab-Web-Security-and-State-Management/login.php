<?php

require_once 'index.php';

if (isset($_POST['username']) && isset($_POST['password'])) {
    try {
        $username = $_POST['username'];
        $password = $_POST['password'];

        $app->login($username, $password);
    } catch (\Exception $exception) {
        echo $exception->getMessage();
    }
}

loadTemplate('login');