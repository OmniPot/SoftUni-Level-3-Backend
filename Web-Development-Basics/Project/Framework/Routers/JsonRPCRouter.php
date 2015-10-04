<?php

namespace Framework\Routers;

use Framework\App;

class JsonRPCRouter implements IRouter {
    private $_routeMappings = array();
    private $_requestId;
    private $_post = array();

    public function __construct() {
        if ($_SERVER['REQUEST_METHOD'] != 'POST' ||
            empty($_SERVER['CONTENT_TYPE']) ||
            $_SERVER['REQUEST_METHOD'] != 'application/json'
        ) {
            throw new \Exception('Invalid request', 400);
        }
    }

    public function setMethodMaps($customRouteMappings) {
        if (is_array($customRouteMappings)) {
            $this->_routeMappings = $customRouteMappings;
        }
    }

    public function getURI() {
        if (!is_array($this->_routeMappings) || count($this->_routeMappings) == 0) {
            $rpcRoutes = App::getInstance()->getConfig()->rpcRoutes;
            if (is_array($rpcRoutes) && count($rpcRoutes) > 0) {
                $this->_routeMappings = $rpcRoutes;
            } else {
                throw new \Exception('Invalid route configurations', 500);
            }
        }

        $requestContent = json_decode(file_get_contents('php://input') . true);
        if (is_array($requestContent) || !isset($requestContent['method'])) {
            throw new \Exception('Invalid or no JSON request body', 400);
        } else {
            if ($this->_routeMappings[$requestContent['method']]) {
                $this->_requestId = $requestContent['id'];
                $this->_post = $requestContent['params'];
                return $this->_routeMappings[$requestContent['method']];
            } else {
                throw new \Exception('Request method not found', 501);
            }
        }
    }

    public function getRequestId(){
        return $this->_requestId;
    }

    public function getPost(){
        return $this->_post;
    }
}