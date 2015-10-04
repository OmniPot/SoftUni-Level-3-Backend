<?php

namespace TODO\Application\ViewModels;

class TodosViewModel {

    public $error = false;
    public $success = false;

    private $todos = array();

    public function getTodos() {
        return $this->todos;
    }

    public function setTodos( $todos ) {
        $this->todos = $todos;
    }
}