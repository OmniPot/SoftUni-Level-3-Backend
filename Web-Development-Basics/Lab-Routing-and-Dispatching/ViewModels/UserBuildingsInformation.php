<?php

namespace Softuni\ViewModels;

class UserBuildingsInformation {

    public $error = false;
    public $success = false;

    private $buildings;
    private $user;

    public function getBuildingsInfo() {
        return $this->buildings;
    }

    public function setBuildingsInfo($buildingsData) {
        $this->buildings = $buildingsData;
    }

    public function setUserInfo($username, $id, $gold, $food) {
        $this->user = [
            'username' => $username,
            'id' => $id,
            'gold' => $gold,
            'food' => $food
        ];
    }

    public function getUserInfo() {
        return $this->user;
    }
}