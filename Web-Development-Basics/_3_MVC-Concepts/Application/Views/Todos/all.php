<?php /** @var $model TODO\Application\ViewModels\TodosViewModel */ ?>
<?= isset( $model ) ? $model->error : ''; ?>

<h3>TODO Items</h3>
<div>
    <ul>
        <?php foreach ( $model->getTodos() as $todo ): ?>
            <li><?= $todo[ 'todo_item' ]; ?>
                <small><a href="delete/<?= $todo[ 'id' ]; ?>">&cross;</a></small>
            </li>
        <?php endforeach; ?>
        <li><a href="add">Add</a></li>
    </ul>
</div>
