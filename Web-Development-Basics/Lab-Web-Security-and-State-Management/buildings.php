<?php

require_once 'index.php';

if (!isset($_SESSION['id'])) {
    header('Location: login.php');
    exit;
}

$buildings = $app->createBuldings();

loadTemplate('buildings', $buildings);

if (isset($_GET['id'])) {
    $buildings->evolve($_GET['id']);
    header('Location: buildings.php');
    exit;
}
