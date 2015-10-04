<?php

session_start();

if (isset($_POST['play-again'])) {
    $_SESSION['correct-number'] = rand($MIN_NUMBER, $MAX_NUMBER);
    $_SESSION['won'] = false;

    header('Location: play.php');
    die;
}