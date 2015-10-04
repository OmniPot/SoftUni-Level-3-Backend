<?php

namespace Softuni\Controllers;

use Softuni\Models\Building;
use Softuni\Models\User;
use Softuni\View;

use Softuni\ViewModels\RegisterInformation;
use Softuni\ViewModels\LoginInformation;
use Softuni\ViewModels\UserBuildingsInformation;
use Softuni\ViewModels\UserProfileInformation;

class UsersController extends BaseController {

    public function login() {
        $viewModel = new LoginInformation();

        if (isset($_POST['username'], $_POST['password'])) {
            try {
                $username = $_POST['username'];
                $password = $_POST['password'];

                $this->initLogin($username, $password);
            } catch (\Exception $exception) {
                $viewModel->error = $exception->getMessage();
                return new View($viewModel);
            }
        }

        return new View();
    }

    public function initLogin($username, $password) {
        $userModel = new User($this->databaseInstance);

        $userId = $userModel->login($username, $password);

        $_SESSION = [];
        $_SESSION['id'] = $userId;

        header('Location: ../profile');
        exit;
    }

    public function register() {
        $viewModel = new RegisterInformation();

        if (isset($_POST['username'], $_POST['password'])) {
            try {
                $username = $_POST['username'];
                $password = $_POST['password'];

                $userModel = new User($this->databaseInstance);
                $userModel->register($username, $password);

                $this->initLogin($username, $password);
            } catch (\Exception $exception) {
                $viewModel->error = $exception->getMessage();
                return new View($viewModel);
            }
        }

        return new View();
    }

    public function profile() {
        if (!$this->isLogged()) {
            header('Location: ../login');
            exit;
        }

        $userModel = new User($this->databaseInstance);
        $userInfo = $userModel->getInfo($_SESSION['id']);

        $viewModel = new UserProfileInformation(
            $userInfo['username'],
            $userInfo['id'],
            null,
            $userInfo['gold'],
            $userInfo['food']
        );

        if ($viewModel) {
            return new View('Users/profile', $viewModel);
        }

        return new View();
    }

    public function buildings() {
        if (!$this->isLogged()) {
            header('Location: ../login');
            exit;
        }

        $buildingModel = new Building($this->databaseInstance);
        $viewModel = new UserBuildingsInformation();

        if (isset($_GET['id']) && is_numeric($_GET['id'])) {
            try {
                $buildingModel->evolve($_GET['id']);
            } catch (\Exception $exception) {
                $viewModel->error = $exception->getMessage();
            }

            header('Location: buildings');
            exit;
        }

        $userModel = new User($this->databaseInstance);

        $buildingData = $buildingModel->all($_SESSION['id']);
        $userData = $userModel->getInfo($_SESSION['id']);

        $viewModel->setBuildingsInfo($buildingData);
        $viewModel->setUserInfo(
            $userData['username'],
            $userData['id'],
            $userData['gold'],
            $userData['food']
        );

        return new View('Users/buildings', $viewModel);
    }
}