<?php

namespace TODO\Application\Models;

class TodoItem extends BaseModel {

    public function getAll( $userId ) {
        $todoItemsQuery =
            "SELECT id, user_id, todo_item
            FROM todos
            WHERE user_id = ?";

        $result = $this->databaseInstance->prepare( $todoItemsQuery );
        $result->execute( [ $userId ] );

        return $result->fetchAll();
    }

    public function add( $userId, $todoText ) {
        if ( strlen( $todoText ) > 5 && strlen( $todoText ) < 250 ) {
            $todoItemsQuery = "INSERT INTO todos (user_id, todo_item) VALUES(?, ?)";

            $result = $this->databaseInstance->prepare( $todoItemsQuery );
            $result->execute( [ $userId, $todoText ] );

            if ( $result ) {
                return true;
            }

            throw new \Exception( 'Failed to add todo' );
        }
    }

    public function delete( $userId, $todoId ) {
        $todoItemsQuery = "DELETE FROM todos WHERE id = ? AND user_id = ?";

        $result = $this->databaseInstance->prepare( $todoItemsQuery );
        $result->execute( [ $todoId, $userId ] );

        if ( $result ) {
            return true;
        }

        throw new \Exception( 'Error while deleting todo item' );
    }
}