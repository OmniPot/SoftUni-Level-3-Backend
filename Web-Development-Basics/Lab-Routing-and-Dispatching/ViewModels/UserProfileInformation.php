<?php

namespace Softuni\ViewModels;

class UserProfileInformation{

    public $error = false;
    public $success = false;

    private $id;
    private $username;
    private $password;
    private $gold;
    private $food;

    public function __construct($username, $password, $id = null, $gold = null, $food = null) {
        $this->setId($id)
            ->setUsername($username)
            ->setPassword($password)
            ->setGold($gold)
            ->setFood($food);
    }

    public function getId() {
        return $this->id;
    }

    public function setId($id) {
        $this->id = $id;
        return $this;
    }

    public function getUsername() {
        return $this->username;
    }

    public function setUsername($username) {
        $this->username = $username;
        return $this;
    }

    public function setPassword($password) {
        $this->password = $password;
        return $this;
    }

    public function getPassword() {
        return $this->password;
    }

    public function getGold() {
        return $this->gold;
    }

    public function setGold($gold) {
        $this->gold = $gold;
        return $this;
    }

    public function getFood() {
        return $this->food;
    }

    public function setFood($food) {
        $this->food = $food;
        return $this;
    }
}