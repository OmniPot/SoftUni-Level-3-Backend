<h1>Hello, <?= htmlspecialchars($data->getUsername()); ?></h1>
<h3>
    Resources:
    <p>Gold: <?= $data->getGold(); ?></p>
    <p>Food: <?= $data->getFood(); ?> </p>
</h3>

<?php if(isset($_GET['error'])) : ?>
    <h2>An error has occurred!</h2>
<?php elseif(isset($_GET['success'])) : ?>
    <h2>Successfully updated profile!</h2>
<?php endif; ?>

<p>Go to: <span><a href="buildings.php">Buildings</a></span></p>
