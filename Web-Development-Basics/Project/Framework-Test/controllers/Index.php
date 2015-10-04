<?php

namespace Controllers;

use Framework\DefaultController;

class Index extends DefaultController {
    public function index2(){

        $this->view->username = 'rumen';

        $this->view->appendToLayout('body', 'admin.index');
        $this->view->appendToLayout('body2', 'index');
        $this->view->display('layouts.admin.default', array('c' => array(1,2,3,5)), false);
    }
}