-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creation time: Jul 23, 2022, 03:51 pm
-- Server version: 10.4.24-MariaDB
-- PHP version: 8.1.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `brawldatabase`
--

-- --------------------------------------------------------

--
-- Table structure `accounts`
--

CREATE TABLE `accounts` (
  `Id` bigint(20) NOT NULL,
  `Trophies` int(11) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`Data`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure `alliances`
--

CREATE TABLE `alliances` (
  `Id` bigint(20) NOT NULL,
  `Name` text NOT NULL,
  `Trophies` int(11) NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`Data`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Saved table indexes
--

--
-- Table indexes `accounts`
--
ALTER TABLE `accounts`
  ADD UNIQUE KEY `Id` (`Id`);

--
-- Table indexes `alliances`
--
ALTER TABLE `alliances`
  ADD UNIQUE KEY `Id` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
