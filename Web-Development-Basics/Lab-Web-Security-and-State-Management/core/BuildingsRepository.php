<?php

namespace Core;

class BuildingsRepository {

    /**
     * @var Database
     */
    private $database;

    /**
     * @var User
     */
    private $user;
    
    public function __construct(Database $database, User $user){
        $this->database = $database;
        $this->user = $user;
    }

    public function getUser(){
        return $this->user;
    }

    public function getBuildings(){
        $query = "
            SELECT
                ubl.building_id AS 'Id',
                b.name AS 'Name',
                ubl.level_id AS 'Level',
                CASE ubl.level_id WHEN 3 THEN 'None' ELSE bl.gold END AS 'Gold',
                CASE ubl.level_id WHEN 3 THEN 'None' ELSE bl.food END AS 'Food'
            FROM users_buildings_levels ubl
            LEFT JOIN buildings b
                ON b.id = ubl.building_id
            LEFT JOIN building_levels bl
                ON bl.building_id = ubl.building_id AND bl.level = ubl.level_id + 1
            WHERE ubl.user_id = ?
        ";

        $result = $this->database->prepare($query);

        $result->execute([$_SESSION['id']]);

        return $result->fetchAll();
    }
    
    public function evolve($buildingId){
        $query = "
            SELECT
                ubl.building_id as 'Id',
                ubl.level_id AS 'Level',
                bl.gold AS 'Gold',
                bl.food AS 'Food'
            FROM users_buildings_levels ubl
            JOIN buildings b
                ON b.id = ubl.building_id
            JOIN building_levels bl
                ON bl.building_id = ubl.building_id AND bl.level = ubl.level_id + 1
            WHERE ubl.user_id = ? AND ubl.building_id = ?
        ";

        $result = $this->database->prepare($query);
        $result->execute([$_SESSION['id'] ,$buildingId]);

        $building = $result->fetch(\PDO::FETCH_ASSOC);

        if ($this->user->getGold() < $building['Gold'] || $this->user->getFood() < $building['Food']) {
            throw new \Exception('Insufficient resource to evolve building');
        }

        if ($building['Level'] == 3) {
            throw new \Exception('Building has reached maximum level and cannot be evolved');
        }

        $resourceUpdate = "
            UPDATE users
            SET gold = ?, food = ?
            WHERE id = ?
        ";
        $result = $this->database->prepare($resourceUpdate);
        $result->execute([
            $this->user->getGold() - $building['Gold'],
            $this->user->getFood() - $building['Food'],
            $_SESSION['id']
        ]);

        if ($result) {
            $buildingUpdate = "
                UPDATE users_buildings_levels
                SET level_id = ?
                WHERE user_id = ? AND building_id = ?
            ";

            $result = $this->database->prepare($buildingUpdate);
            $result->execute([$building['Level'] + 1, $_SESSION['id'], $buildingId]);

            if ($result) {
                return true;
            }

            throw new \Exception('Error occurred while upgrading building');
        }

        throw new \Exception('Error occurred while upgrading building');
    }
}