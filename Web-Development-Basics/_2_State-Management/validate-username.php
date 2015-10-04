<?php
    include('constants.php');

    if (isset($_POST['submit'])) {
        if (!empty($_POST['username'])) {
            session_start();
            $_SESSION['username'] = $_POST['username'];
            $_SESSION['correct-number'] = rand($MIN_NUMBER, $MAX_NUMBER);

            header('Location: play.php');
            die;
        } else {
            echo $NO_USERNAME_MESSAGE;
        }
    }
