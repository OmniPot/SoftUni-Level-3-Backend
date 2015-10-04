<?php /** @var $model Softuni\ViewModels\UserBuildingsInformation */ ?>
<?= isset($model) ? $model->error : ''; ?>
<?= isset($_SESSION['error']) ? $_SESSION['error'] : ''; ?>

<h1>Buildings</h1>

<h3>
    Resources:
    <p>Gold: <?= $model->getUserInfo()['gold']; ?></p>
    <p>Food: <?= $model->getUserInfo()['food']; ?></p>
</h3>

<table border="1">
    <tr>
        <td>Building Name</td>
        <td>Current Level</td>
        <td>Evolve Gold Cost</td>
        <td>Evolve Food Cost</td>
        <td>Action</td>
    </tr>
    <?php foreach($model->getBuildingsInfo() as $building): ?>
        <tr>
            <td><?= $building['Name']; ?></td>
            <td><?= $building['Level']; ?></td>
            <td><?= $building['GoldCost']; ?></td>
            <td><?= $building['FoodCost']; ?></td>
            <td><a href="?id=<?= $building['Id']; ?>">Evolve</a></td>
        </tr>
    <?php endforeach; ?>
</table>