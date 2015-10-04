<?php /** @var $model Softuni\ViewModels\UserProfileInformation */ ?>
<?= isset($model) ? $model->error : ''; ?>

<h1>Hello, <?= htmlspecialchars($model->getUsername()); ?></h1>
<h3>
    Resources:
    <p>Gold: <?= $model->getGold(); ?></p>
    <p>Food: <?= $model->getFood(); ?></p>
</h3>

<p>Go to: <span><a href="buildings">Buildings</a></span></p>