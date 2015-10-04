<?php

namespace Framework;

class View {
    private static $_instance = null;
    private $___viewPath = null;
    private $___viewDir = null;
    private $___data = array();
    private $___extension = '.php';
    private $___layoutParts = array();
    private $___layoutData = array();

    private function __construct() {
        $this->___viewPath = App::getInstance()->getConfig()->app['viewDirectory'];
        if ($this->___viewPath == null) {
            $this->___viewPath = realpath('../views/');
        }
    }

    public function __set($name, $value) {
        $this->___data[$name] = $value;
    }

    public function __get($name) {
        return $this->___data[$name];
    }

    public function display($templateName, $data = array(), $returnAsString = false) {
        if (is_array($data)) {
            $this->___data = array_merge($this->___data, $data);
        }

        if (count($this->___layoutParts) > 0) {
            foreach ($this->___layoutParts as $partKey => $partValue) {
                $renderedLayout = $this->_includeFile($partValue);
                if ($renderedLayout) {
                    $this->___layoutData[$partKey] = $renderedLayout;
                }
            }
        }

        if ($returnAsString) {
            return $this->_includeFile($templateName);
        } else {
            echo $this->_includeFile($templateName);
        }
    }

    public function getLayoutData($name) {
        return $this->___layoutData[$name];
    }

    public function appendToLayout($key, $template) {
        if ($key && $template) {
            $this->___layoutParts[$key] = $template;
        } else {
            throw new \Exception('Layout requires valid key and template', 500);
        }
    }

    public function _includeFile($fileName) {
        if ($this->___viewDir == null) {
            $this->setViewDirectory($this->___viewPath);
        }

        $___fullViewPath = $this->___viewDir . str_replace('.', DIRECTORY_SEPARATOR, $fileName) . $this->___extension;
        if (file_exists($___fullViewPath) && is_readable($___fullViewPath)) {
            ob_start();
            include $___fullViewPath;
            return ob_get_clean();
        } else {
            throw new \Exception('View ' . $fileName . ' cannot be included', 500);
        }
    }

    public function setViewDirectory($path) {
        $path = trim($path);
        if ($path) {
            $path = realpath($path) . DIRECTORY_SEPARATOR;
            if (is_dir($path) && is_readable($path)) {
                $this->___viewDir = $path;
            } else {
                throw new \Exception('Invalid view path', 500);
            }
        } else {
            throw new \Exception('No view path specified', 500);
        }
    }

    /**
     * @return \Framework\View
     */
    public static function getInstance() {
        if (self::$_instance == null) {
            self::$_instance = new View();
        }

        return self::$_instance;
    }
}