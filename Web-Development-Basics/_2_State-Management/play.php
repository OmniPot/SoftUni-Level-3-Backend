<?php
    include('header.php');
    include('constants.php');

    session_start();
?>
    <div>
        <form method="post">
            <?php if($_SESSION['won']) : ?>
                <p><?php echo $WIN_MESSAGE . htmlspecialchars($_SESSION['username']). '!' ?></p>
                <input type="submit" name="play-again" value="Play again">
            <?php else : ?>
                <?php echo $GUESS_A_NUMBER_MESSAGE ?>
                <input type="text" name="guess-number" placeholder="Number">
                <input type="submit" name="submit" value="Make a guess">
            <?php endif ; ?>
        </form>
    </div>
<?php
    include('footer.php');
    include('restart.php');
    include('process-guess.php');
?>
