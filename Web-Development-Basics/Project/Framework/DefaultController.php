<?php

namespace Framework;

class DefaultController {
    public $app;
    public $config;
    public $view;
    public $input;

    public function __construct() {
        $this->app = App::getInstance();
        $this->config = $this->app->getConfig();
        $this->view = View::getInstance();
        $this->input = InputData::getInstance();
    }

    public function jsonResponse() {

    }
}