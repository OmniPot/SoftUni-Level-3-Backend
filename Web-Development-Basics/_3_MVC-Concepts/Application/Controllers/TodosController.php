<?php

namespace TODO\Application\Controllers;

use TODO\Application\Models\TodoItem;
use TODO\Framework\View;

use TODO\Application\ViewModels\TodosViewModel;

class TodosController extends BaseController {

    public function all() {
        if ( !$this->isLogged() ) {
            $this->redirect( $this->unauthorizedLocation );
        }

        $userId = $_SESSION[ 'id' ];

        $todoModel = new TodoItem( $this->databaseInstance );
        $todosViewModel = new TodosViewModel();

        $todos = $todoModel->getAll( $userId );
        $todosViewModel->setTodos( $todos );

        return new View( $todosViewModel );
    }

    public function add() {
        if ( !$this->isLogged() ) {
            $this->redirect( $this->unauthorizedLocation );
        }

        $todoModel = new TodoItem( $this->databaseInstance );
        $todosViewModel = new TodosViewModel();

        if ( isset( $_POST[ 'todo_text' ] ) ) {
            $userId = $_SESSION[ 'id' ];
            $todoText = $_POST[ 'todo_text' ];

            $todoModel = new TodoItem( $this->databaseInstance );
            try {
                $todoModel->add( $userId, $todoText );
            } catch ( \Exception $ex ) {
                $todosViewModel->error = $ex->getMessage();
            }

            $this->redirect( 'todos/all' );
        }

        return new View( $todosViewModel );
    }

    public function delete() {
        if ( !$this->isLogged() ) {
            $this->redirect( $this->unauthorizedLocation );
        }

        $todoModel = new TodoItem( $this->databaseInstance );

        if ( isset( $this->requestParams[ 0 ] ) && is_numeric( $this->requestParams[ 0 ] ) ) {
            $todoId = $this->requestParams[ 0 ];
            $userId = $_SESSION[ 'id' ];

            try {
                $todoModel->delete( $userId, $todoId );
            } catch ( \Exception $exception ) {
                $todoModel->error = $exception->getMessage();
            }
        }

        $this->redirect( 'todos/all' );
    }
}