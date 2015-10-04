<?php

namespace Softuni\Models;

use Softuni\Core\Database;

class User {

    /** @var $database Database */
    private $database;

    const GOLD_DEFAULT = 1500;
    const FOOD_DEFAULT = 1500;

    public function __construct($databaseInstance) {
        $this->database = $databaseInstance;
    }

    public function register($username, $password) {

        if ($this->exists($username)) {
            throw new \Exception('Username already taken');
        }

        $registerUserQuery =
            "INSERT INTO users(username, password, gold, food)
            VALUES(?, ?, ?, ?)";
        $result = $this->database->prepare($registerUserQuery);

        $result->execute([
            $username,
            password_hash($password, PASSWORD_DEFAULT),
            self::GOLD_DEFAULT,
            self::FOOD_DEFAULT
        ]);

        if ($result->rowCount() > 0) {
            $userId = $this->database->lastId();

            $this->database->query("
                INSERT INTO users_buildings_levels (user_id, building_id, level_id)
                SELECT $userId, id, 0
                FROM buildings
            ");

            $this->login($username, $password);
        } else {
            throw new \Exception('Unsuccessful registration');
        }
    }

    public function exists($username) {

        $findUserQuery =
            "SELECT id
            FROM users
            WHERE username = ?";
        $result = $this->database->prepare($findUserQuery);
        $result->execute([$username]);

        return $result->rowCount() > 0;
    }

    /**
     * @param $username
     * @param $password
     * @return User
     * @throws \Exception
     */
    public function login($username, $password) {

        $query =
            "SELECT id, password
            FROM users
            WHERE username = ?";

        $result = $this->database->prepare($query);
        $result->execute([$username]);

        if ($result->rowCount() > 0) {
            $userRow = $result->fetch();

            if (password_verify($password, $userRow['password'])) {
                return $userRow['id'];
            }

            throw new \Exception('Wrong password');
        }

        throw new \Exception('Invalid login data');
    }

    public function getInfo($userId) {

        $query =
            "SELECT id, username, password, gold, food
            FROM users
            WHERE id = ?";
        $result = $this->database->prepare($query);

        $result->execute([$userId]);

        return $result->fetch();
    }

    public function edit($newUsername, $newPassword, $id) {

        $updateQuery =
            "UPDATE users
            SET password = ?, username = ?
            WHERE id = ?";
        $result = $this->database->prepare($updateQuery);

        $result->execute([
            $newUsername,
            $newPassword,
            $id
        ]);

        return $result->rowCount() > 0;
    }
}