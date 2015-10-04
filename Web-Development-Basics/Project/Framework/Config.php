<?php

namespace Framework;

class Config {
    private static $_instance = null;

    private $_configFolder = null;
    private $_configArray = null;

    private function __construct() {

    }

    public function getConfigFolder() {
        return $this->_configFolder;
    }

    public function setConfigFolder($configFolder) {
        if (!$configFolder) {
            throw new \Exception('Empty config folder path:');
        }

        $_configFolder = realpath($configFolder);
        if ($configFolder != false && is_dir($_configFolder) && is_readable($_configFolder)) {
            $this->_configArray = array();
            $this->_configFolder = $_configFolder . DIRECTORY_SEPARATOR;

            $namespace = $this->app['namespaces'];
            if (is_array($namespace)) {
                Loader::registerNamespaces($namespace);
            }
        } else {
            throw new \Exception('Config directory read error:' . $configFolder);
        }
    }

    public function includeConfigFile($path) {
        if (!$path) {
            throw new \Exception;
        }

        $_file = realpath($path);
        if ($_file != false && is_file($_file) && is_readable($_file)) {
            $_basename = explode('.php', basename($_file))[0];
            $this->_configArray[$_basename] = include $_file;
        } else {
            throw new \Exception('Config file read error:' . $path);
        }
    }

    public function __get($name) {
        if (!$this->_configArray[$name]) {
            $this->includeConfigFile($this->_configFolder . $name . '.php');
        }

        if (array_key_exists($name, $this->_configArray)) {
            return $this->_configArray[$name];
        }

        return null;
    }

    /**
     * @return \Framework\Config
     */
    public static function getInstance() {
        if (self::$_instance == null) {
            self::$_instance = new Config();
        }

        return self::$_instance;
    }
}