<h1>Hello, <?= htmlspecialchars($data->getUsername()); ?></h1>
<h3>
    Resources:
    <p>Gold: <?= $data->getGold(); ?></p>
    <p>Food: <?= $data->getFood(); ?> </p>
</h3>

<form method="post">
    <div>
        <input type="text" name="username" value="<?= htmlspecialchars($data->getUsername()); ?>" placeholder="Username">
        <input type="password" name="password" placeholder="New password">
        <input type="password" name="confirm" placeholder="Confirm password">
        <input type="submit" name="edit" value="Edit">
    </div>
</form>

<p>Go to: <span><a href="buildings.php">Buildings</a></span></p>