<?php

namespace Softuni;

use Softuni\Controllers\BaseController;

class App {

    private $controllerName;
    private $actionName;
    private $requestParams = [];

    /**
     * @var $controller BaseController
     */
    private $controller;

    const CONTROLLERS_NAMESPACE = 'Softuni\\Controllers\\';
    const CONTROLLERS_SUFFIX = 'Controller';

    public function __construct($controllerName, $actionName, $requestParams = []) {
        $this->controllerName = $controllerName;
        $this->actionName = $actionName;
        $this->requestParams = $requestParams;
    }

    public function start() {

        $this->initController();

        View::$controllerName = $this->controllerName;
        View::$actionName = $this->actionName;

        call_user_func_array(
            [
                $this->controller,
                $this->actionName
            ],
            $this->requestParams
        );
    }

    private function initController() {

        $controllerName =
            self::CONTROLLERS_NAMESPACE
            . $this->controllerName
            . self::CONTROLLERS_SUFFIX;

        $this->controller = new $controllerName($this->requestParams);
    }
}