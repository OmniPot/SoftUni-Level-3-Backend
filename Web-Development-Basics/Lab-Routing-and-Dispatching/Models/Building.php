<?php
namespace Softuni\Models;

use Softuni\Core\Database;

class Building {

    /** @var $database Database */
    private $database;

    public function __construct($databaseInstance) {
        $this->database = $databaseInstance;
    }

    public function all($userId) {

        $query = "
            SELECT
                ubl.building_id AS 'Id',
                b.name AS 'Name',
                ubl.level_id AS 'Level',
                CASE ubl.level_id WHEN b.max_level THEN 'None' ELSE bl.gold END AS 'GoldCost',
                CASE ubl.level_id WHEN b.max_level THEN 'None' ELSE bl.food END AS 'FoodCost'
            FROM users_buildings_levels ubl
            LEFT JOIN buildings b
                ON b.id = ubl.building_id
            LEFT JOIN building_levels bl
                ON bl.building_id = ubl.building_id AND bl.level = ubl.level_id + 1
            WHERE ubl.user_id = ?
        ";

        $result = $this->database->prepare($query);
        $result->execute([$userId]);

        return $result->fetchAll();
    }

    public function evolve($buildingId) {

        $getQuery =
            "SELECT
               u.gold as 'UserGold',
               u.food as 'UserFood',
               bl.level - 1 AS 'Level',
               b.max_level AS 'MaxLevel',
               bl.gold AS 'GoldCost',
               bl.food AS 'FoodCost'
            FROM users_buildings_levels ubl
            JOIN users u
               ON u.id = ubl.user_id
            JOIN buildings b
               ON b.id = ubl.building_id
            JOIN building_levels bl
               ON bl.building_id = ubl.building_id AND bl.level = ubl.level_id + 1
            WHERE ubl.user_id = ? AND ubl.building_id = ?";

        // Get data needed to evolve building
        $getStatement = $this->database->prepare($getQuery);
        $getStatement->execute([$_SESSION['id'], $buildingId]);
        $getResult = $getStatement->fetch(\PDO::FETCH_ASSOC);

        if ($getResult['Level'] == $getResult['MaxLevel']) {
            throw new \Exception('Building has reached maximum level and cannot be evolved');
        }

        if ($getResult['UserGold'] < $getResult['GoldCost'] ||
            $getResult['UserFood'] < $getResult['FoodCost']) {
            throw new \Exception('Insufficient resource to evolve building');
        }

        // Update user resources
        $resourceUpdateQuery = "UPDATE users SET gold = ?, food =  ? WHERE id = ?";
        $resourceResult = $this->database->prepare($resourceUpdateQuery);
        $resourceResult->execute([
            $getResult['UserGold'] - $getResult['GoldCost'],
            $getResult['UserFood'] - $getResult['FoodCost'],
            $_SESSION['id']
        ]);

        // Update building level
        $buildingUpdateQuery = "UPDATE users_buildings_levels SET level_id = ? WHERE user_id = ? AND building_id = ?";
        $updateBuildingResult = $this->database->prepare($buildingUpdateQuery);
        $updateBuildingResult->execute([$getResult['Level'] + 1, $_SESSION['id'], $buildingId]);
    }
}