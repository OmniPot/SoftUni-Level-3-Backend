<?php

include('constants.php');

session_start();

if (isset($_POST['submit'])) {
    if ($_POST['guess-number'] != '') {
        if (!is_numeric($_POST['guess-number'])) {
            echo $INVALID_NUMBER;
        } else if ($_POST['guess-number'] < $MIN_NUMBER || $_POST['guess-number'] > $MAX_NUMBER) {
            echo $NUMBER_OUT_OF_RANGE;
        } else {

            if ($_POST['guess-number'] > $_SESSION['correct-number']) {
                echo $TRY_SMALLER_NUMBER_MESSAGE;
            } else if ($_POST['guess-number'] < $_SESSION['correct-number']) {
                echo $TRY_BIGGER_NUMBER_MESSAGE;
            } else {
                $_SESSION['won'] = true;
                header('Location: play.php'); die;
            }
        }
    } else {
        echo $NO_NUMBER_MESSAGE;
    }
}
