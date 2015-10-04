<?php
    include('header.php');
    include('constants.php');
?>
<div>
    <form method="post">
        <p><?php echo $NO_USERNAME_MESSAGE; ?></p>
        <input type="text" name="username" placeholder="Username">
        <input type="submit" name="submit" value="Play">
    </form>
</div>
<div>
    <?php
        include('validate-username.php');
    ?>
</div>

<?php
    include('footer.php');
?>
