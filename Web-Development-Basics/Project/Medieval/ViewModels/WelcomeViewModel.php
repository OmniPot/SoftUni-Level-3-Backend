<?php

namespace Medieval\ViewModels;

class WelcomeViewModel {

    public $error;
    public $success;

    private $username;

    public function getUsername() {
        return $this->username;
    }

    public function setUsername( $username ) {
        $this->username = $username;
    }
}