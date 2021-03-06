<?php

namespace TODO\Framework;

use TODO\Application\Config\Config;

class View {

    public static $controllerName;
    public static $actionName;

    const PARAMS_COUNT_MODEL_AND_VIEW = 2;
    const PARAMS_COUNT_MODEL_OR_VIEW = 1;

    public function __construct() {
        $args = func_get_args();

        if ( count( $args ) == self::PARAMS_COUNT_MODEL_AND_VIEW ) {
            $view = $args[ 0 ];
            $model = $args[ 1 ];
            $this->initModelView( $view, $model );
        } else {
            $model = isset( $args[ 0 ] ) ? $args[ 0 ] : null;
            $this->initModelOnly( $model );
        }
    }

    private function initModelOnly( $model ) {
        require_once
            Config::VIEW_FOLDER
            . DIRECTORY_SEPARATOR
            . self::$controllerName
            . DIRECTORY_SEPARATOR
            . self::$actionName
            . Config::PHP_EXTENSION;
    }

    private function initModelView( $view, $model ) {
        require_once
            Config::VIEW_FOLDER
            . DIRECTORY_SEPARATOR
            . $view
            . Config::PHP_EXTENSION;
    }
}