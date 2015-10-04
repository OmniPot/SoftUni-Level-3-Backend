<?php

namespace Framework\Routers;

class DefaultRouter implements IRouter {
    public function getURI() {
        $_uri = substr($_SERVER['PHP_SELF'], strlen($_SERVER['SCRIPT_NAME']) + 1);
        return $_uri;
    }

    public function getPost() {
        return $_POST;
    }
}