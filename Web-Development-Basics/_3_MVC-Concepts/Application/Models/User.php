<?php

namespace TODO\Application\Models;

class User extends BaseModel {

    public function register( $username, $password ) {

        if ( $this->exists( $username ) ) {
            throw new \Exception( 'Username already taken' );
        }

        $registerUserQuery = "INSERT INTO users(username, password_hash) VALUES(?, ?)";
        $result = $this->databaseInstance->prepare( $registerUserQuery );

        $result->execute( [
            $username,
            password_hash( $password, PASSWORD_DEFAULT )
        ] );

        if ( $result->rowCount() > 0 ) {
            $userId = $this->databaseInstance->lastId();

            $this->login( $username, $password );
        } else {
            throw new \Exception( 'Unsuccessful registration' );
        }
    }

    public function exists( $username ) {

        $findUserQuery =
            "SELECT id
            FROM users
            WHERE username = ?";
        $result = $this->databaseInstance->prepare( $findUserQuery );
        $result->execute( [ $username ] );

        return $result->rowCount() > 0;
    }

    /**
     * @param $username
     * @param $password
     * @return User
     * @throws \Exception
     */
    public function login( $username, $password ) {

        $query = "SELECT id, password_hash FROM users WHERE username = ?";

        $result = $this->databaseInstance->prepare( $query );
        $result->execute( [ $username ] );

        if ( $result->rowCount() > 0 ) {
            $userRow = $result->fetch();

            if ( password_verify( $password, $userRow[ 'password_hash' ] ) ) {
                return $userRow[ 'id' ];
            }

            throw new \Exception( 'Wrong password' );
        }

        throw new \Exception( 'Invalid login data' );
    }

    public function getInfo( $userId ) {

        $query =
            "SELECT id, username, password_hash
            FROM users
            WHERE id = ?";
        $result = $this->databaseInstance->prepare( $query );

        $result->execute( [ $userId ] );

        return $result->fetch();
    }

    public function edit( $newUsername, $newPassword, $id ) {

        $updateQuery =
            "UPDATE users
            SET password_hash = ?, username = ?
            WHERE id = ?";
        $result = $this->databaseInstance->prepare( $updateQuery );

        $result->execute( [
            $newUsername,
            password_hash( $newPassword, PASSWORD_DEFAULT ),
            $id
        ] );

        return $result->rowCount() > 0;
    }
}