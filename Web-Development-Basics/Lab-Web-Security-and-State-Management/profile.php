<?php

require_once 'index.php';

if (!isset($_SESSION['id'])) {
    header('Location: login.php');
    exit;
}

loadTemplate('profile', $app->getUser());

