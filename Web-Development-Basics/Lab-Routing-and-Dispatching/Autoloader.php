<?php

namespace Softuni;

class Autoloader {

    public static function init(){
        spl_autoload_register(function ($class) {
            $classPath = str_replace('\\', DIRECTORY_SEPARATOR, $class);
            $classPath = str_replace('Softuni\\', '', $class . '.php');

            require_once $classPath;
        });
    }
}