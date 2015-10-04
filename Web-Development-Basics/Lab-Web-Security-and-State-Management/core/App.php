<?php
namespace Core;

class App {

    /**
     * @var Database
     */
    private $database;

    /**
     * @var User
     */
    private $user;

    /**
     * @var BuildingsRepository
     */
    private $buildingsRepository;

    public function __construct(Database $database) {
        $this->database = $database;
    }

    public function isLogged() {
        return isset($_SESSION['id']);
    }

    /**
     * @param $username
     * @return bool
     */
    public function userExists($username) {
        $findUserQuery = "SELECT id FROM users WHERE username = ?";
        $result = $this->database->prepare($findUserQuery);
        $result->execute([$username]);

        return $result->rowCount() > 0;
    }

    public function register($username, $password) {
        if ($this->userExists($username)) {
            throw new \Exception('Username already taken');
        }

        $registerUserQuery = "INSERT INTO users(username, password, gold, food) VALUES(?, ?, ?, ?)";
        $result = $this->database->prepare($registerUserQuery);

        $result->execute([
            $username,
            password_hash($password, PASSWORD_DEFAULT),
            User::GOLD_DEFAULT,
            User::FOOD_DEFAULT
        ]);

        if ($result->rowCount() > 0) {
            $userId = $this->database->lastId();

            $this->database->query("
                INSERT INTO users_buildings_levels (user_id, building_id, level_id)
                SELECT $userId, id, 0
                FROM buildings
            ");

            $this->login($username, $password);
        }

        throw new \Exception('Unsuccessful registration');
    }

    /**
     * @param $username
     * @param $password
     * @return User
     * @throws \Exception
     */
    public function login($username, $password) {

        $query = "
            SELECT id, password
            FROM users
            WHERE username = ?
        ";

        $result = $this->database->prepare($query);
        $result->execute([$username]);

        if ($result->rowCount() > 0) {
            $userRow = $result->fetch();

            if (password_verify($password, $userRow['password'])) {
                session_start();

                $_SESSION['id'] = $userRow['id'];
                $this->getUserInfo($_SESSION['id']);

                header('Location: profile.php');
            }

            throw new \Exception('Invalid login data');
        }

        throw new \Exception('Invalid login data');
    }

    public function getUserInfo($userId) {
        $query = "SELECT id, username, password, gold, food
                  FROM users
                  WHERE id = ?";
        $result = $this->database->prepare($query);

        $result->execute([$userId]);

        return $result->fetch();
    }

    public function getUser() {
        if ($this->user != null) {
            return $this->user;
        }

        if ($this->isLogged()) {
            $userRow = $this->getUserInfo($_SESSION['id']);
            $this->user = $user = new User(
                $userRow['username'],
                $userRow['password'],
                $userRow['id'],
                $userRow['gold'],
                $userRow['food']
            );

            return $this->user;
        }

        return null;
    }

    public function editUser(User $newData) {
        $updateQuery = "UPDATE users SET password = ?, username = ? WHERE id = ?";
        $result = $this->database->prepare($updateQuery);

        $result->execute([
            $newData->getPassword(),
            $newData->getUsername(),
            $newData->getId()
        ]);

        return $result->rowCount() > 0;
    }

    public function createBuldings(){
        if ($this->buildingsRepository == null) {
            $this->buildingsRepository = new BuildingsRepository($this->database, $this->getUser());
        }

        return $this->buildingsRepository;
    }
}