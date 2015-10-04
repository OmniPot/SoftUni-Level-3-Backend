<?php

namespace Framework;

class InputData {
    private static $_instance = null;
    private $_get = array();
    private $_post = array();
    private $_cookies = array();

    private function __construct() {
        $this->_cookies = $_COOKIE;
    }

    public function setPost($postArray) {
        if (is_array($postArray)) {
            $this->_post = $postArray;
        }
    }

    public function setGet($getArray) {
        if (is_array($getArray)) {
            $this->_post = $getArray;
        }
    }

    private function hasGet($id) {
        return array_key_exists($id, $this->_get);
    }

    private function hasPost($name) {
        return array_key_exists($name, $this->_post);
    }

    private function hasCookie($name) {
        return array_key_exists($name, $this->_cookies);
    }

    public function get($id, $normalizationTypes = null, $default = null) {
        if ($this->hasGet($id)) {
            if ($normalizationTypes != null) {
                return Utilities::normalize($this->_get[$id], $normalizationTypes);
            }

            return $this->_get[$id];
        }

        return $default;
    }

    public function post($name, $normalizationTypes = null, $default = null) {
        if ($this->hasPost($name)) {
            if ($normalizationTypes != null) {
                return Utilities::normalize($this->_post[$name], $normalizationTypes);
            }

            return $this->_post[$name];
        }

        return $default;
    }

    public function cookies($name, $normalizationTypes = null, $default = null) {
        if ($this->hasCookie($name)) {
            if ($normalizationTypes != null) {
                return Utilities::normalize($this->_cookies[$name], $normalizationTypes);
            }

            return $this->_cookies[$name];
        }

        return $default;
    }

    /**
     * @return \Framework\InputData
     */
    public static function getInstance() {
        if (self::$_instance == null) {
            self::$_instance = new InputData();
        }

        return self::$_instance;
    }
}