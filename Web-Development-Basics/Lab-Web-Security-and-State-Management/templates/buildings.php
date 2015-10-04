<h1>Buildings</h1>

<h3>
    Resources:
    <p>Gold: <?= $data->getUser()->getGold(); ?></p>
    <p>Food: <?= $data->getUser()->getFood(); ?> </p>
</h3>

<table border="1">
    <tr>
        <td>Building Name</td>
        <td>Current Level</td>
        <td>Evolve Gold Cost</td>
        <td>Evolve Food Cost</td>
        <td>Action</td>
    </tr>
    <?php foreach($data->getBuildings() as $building): ?>
        <tr>
            <td><?= $building['Name']; ?></td>
            <td><?= $building['Level']; ?></td>
            <td><?= $building['Gold']; ?></td>
            <td><?= $building['Food']; ?></td>
            <td><a href="buildings.php?id=<?= $building['Id']; ?>">Evolve</a></td>
        </tr>
    <?php endforeach; ?>
</table>