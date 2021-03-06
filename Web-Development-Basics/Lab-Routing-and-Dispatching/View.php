<?php

namespace Softuni;

class View {

    public static $controllerName;
    public static $actionName;

    const PARAMS_COUNT_MODEL_AND_VIEW = 2;
    const PARAMS_COUNT_MODEL_OR_VIEW = 1;

    const VIEW_FOLDER = 'Views';
    const VIEW_EXTENSIONS = '.php';
    
    public function __construct(){
        $args = func_get_args();

        if (count($args) == self::PARAMS_COUNT_MODEL_AND_VIEW) {
            $view = $args[0];
            $model = $args[1];
            $this->initModelView($view, $model);
        } else {
            $model = isset($args[0]) ? $args[0] : null;
            $this->initModelOnly($model);
        }
    }
    
    private function initModelOnly($model){

        require_once
            self::VIEW_FOLDER
            . DIRECTORY_SEPARATOR
            . self::$controllerName
            . DIRECTORY_SEPARATOR
            . self::$actionName
            . self::VIEW_EXTENSIONS;
    }
    
    private function initModelView($view, $model){

        require_once
            self::VIEW_FOLDER
            . DIRECTORY_SEPARATOR
            . $view
            . self::VIEW_EXTENSIONS;
    }
}