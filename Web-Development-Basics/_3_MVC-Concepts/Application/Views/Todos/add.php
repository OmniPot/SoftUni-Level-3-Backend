<?php /** @var $model TODO\Application\ViewModels\TodosViewModel */ ?>
<?= isset( $model ) ? $model->error : ''; ?>

<h3>Add TODO Item</h3>
<div>
    <form action="" method="POST">
        <input type="text" name="todo_text" placeholder="Todo text">
        <input type="submit" name="add" value="Add TODO">
        <span> or <a href="all">Cancel</a></span>
    </form>
</div>
