﻿// ***********************************************************************
// Assembly         : ETL
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-04-2020
// ***********************************************************************
// <copyright file="Converters.cs" company="">
//     Copyright ©  2019 Mariusz Owczarski
// </copyright>
// <summary></summary>
// ***********************************************************************
using Contract.Enum;
using System;
using System.Text.RegularExpressions;

namespace ETL.Helpers
{
    /// <summary>
    /// Class Converters.
    /// </summary>
    public class Converters
    {
        // KONVERTERY SLUZA GLOWNIE W PROCESIE TRANSFORMACJI 
        // KONWERTUJA DANE W POSTACI LANCUCHA ZNAKOW NA ROZNE TYPY (INT, DECIMAL, ENUM, DATETIME)

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>System.Decimal.</returns>
        public static decimal ConvertToDecimal(dynamic it)
        {
            if (it == null) return 0;
            else return Convert.ToDecimal(Regex.Match(Convert.ToString(it).Replace(" ", ""), @"\d+").Value);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>System.String.</returns>
        public static string ConvertToString(dynamic it)
        {
            if (it == null) return null;
            else return Convert.ToString(it);
        }

        /// <summary>
        /// Converts to int.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt;.</returns>
        public static int? ConvertToInt(dynamic it)
        {
            if (it == null) return null;
            else return Convert.ToInt32(Regex.Match(Convert.ToString(it).Replace(" ", ""), @"\d+").Value);
        }

        /// <summary>
        /// Converts to type byte.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>System.Byte.</returns>
        public static byte ConvertToTypeByte(dynamic it)
        {
            if (it == null) return 0;
            var result = Convert.ToString(it);

            switch (result)
            {
                case "Dramat":
                    return 1;
                case "Akcja":
                    return 2;
                case "Dokumentalny":
                    return 3;
                case "Familijny":
                    return 4;
                case "Fantasy":
                    return 5;
                case "Horror":
                    return 6;
                case "Kryminał":
                    return 7;
                case "Melodramat":
                    return 8;
                case "Niemy":
                    return 9;
                case "Przygodowy":
                    return 10;
                case "Romans":
                    return 11;
                case "Sci-Fi":
                    return 12;
                case "Thriller":
                    return 13;
                case "Animowany":
                    return 14;
                case "Komedia":
                    return 15;
                default:
                    return 0;
            };
        }

        /// <summary>
        /// Converts the type of to movie.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>MOVIE_TYPE.</returns>
        public static MOVIE_TYPE ConvertToMovieType(dynamic it)
        {
            if (it == null) return MOVIE_TYPE.UNKNOWN;
            var type = Convert.ToInt32(it);

            switch (type)
            {
                case 0:
                    return MOVIE_TYPE.UNKNOWN;
                case 1:
                    return MOVIE_TYPE.DRAMAT;
                case 2:
                    return MOVIE_TYPE.AKCJA;
                case 3:
                    return MOVIE_TYPE.DOKUMENTALNY;
                case 4:
                    return MOVIE_TYPE.FAMILIJNY;
                case 5:
                    return MOVIE_TYPE.FANTASY;
                case 6:
                    return MOVIE_TYPE.HORROR;
                case 7:
                    return MOVIE_TYPE.KRYMIMAL;
                case 8:
                    return MOVIE_TYPE.MELODRAMAT;
                case 9:
                    return MOVIE_TYPE.NIEMY;
                case 10:
                    return MOVIE_TYPE.PRZYGODOWY;
                case 11:
                    return MOVIE_TYPE.ROMANS;
                case 12:
                    return MOVIE_TYPE.SCIFI;
                case 13:
                    return MOVIE_TYPE.THRILLER;
                case 14:
                    return MOVIE_TYPE.ANIMOWANY;
                case 15:
                    return MOVIE_TYPE.KOMEDIA;
                default:
                    return MOVIE_TYPE.UNKNOWN;
            }
        }

        /// <summary>
        /// Converts to date time.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ConvertToDateTime(dynamic it)
        {
            if (it == null) return new DateTime();

            var dataString = Convert.ToString(it).Split(' ');
            var month = 1;
            var year = 1000;
            var day = 1;

            try
            {
                year = Convert.ToInt32(dataString[4]);
                day = Convert.ToInt32(dataString[2]);
            }
            catch{}
                
            switch (dataString[3])
            {
                case "stycznia": case "styczeń":
                    month = 1;
                    break;
                case "lutego": case "luty":
                    month = 2;
                    break;
                case "marca": case "marzec":
                    month = 3;
                    break;
                case "kwietnia": case "kwiecień":
                    month = 4;
                    break;
                case "maja": case "maj":
                    month = 5;
                    break;
                case "czerwca": case "czerwiec":
                    month = 6;
                    break;
                case "lipiec": case "lipca":
                    month = 7;
                    break;
                case "sierpnia": case "sierpień":
                    month = 8;
                    break;
                case "wrzesień": case "września":
                    month = 9;
                    break;
                case "październik": case "października":
                    month = 10;
                    break;
                case "listopada": case "listopad":
                    month = 11;
                    break;
                case "grudnia": case "grudzień":
                    month = 12;
                    break;
                default:
                    month = 1;
                    break;
            }
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Converts to production.
        /// </summary>
        /// <param name="it">It.</param>
        /// <returns>PRODUCTION_COUNTRY.</returns>
        public static PRODUCTION_COUNTRY ConvertToProduction(dynamic it)
        {
            if (it == null) return PRODUCTION_COUNTRY.UNKNOWN;

            var country = Convert.ToString(it);

            switch (country)
            {
                case "Polska":
                    return PRODUCTION_COUNTRY.POLSKA;
                case "USA":
                    return PRODUCTION_COUNTRY.USA;
                case "Kanada":
                    return PRODUCTION_COUNTRY.KANADA;
                case "Hiszpania":
                    return PRODUCTION_COUNTRY.HISZPANIA;
                case "Australia":
                    return PRODUCTION_COUNTRY.AUSTRALIA;
                case "Anglia":
                    return PRODUCTION_COUNTRY.ANGLIA;
                case "Argentyna":
                    return PRODUCTION_COUNTRY.ARGENTYNA;
                case "Filipiny":
                    return PRODUCTION_COUNTRY.FILIPINY;
                case "Francja":
                    return PRODUCTION_COUNTRY.FRANCJA;
                case "Grecja":
                    return PRODUCTION_COUNTRY.GRECJA;
                case "HonKong":
                    return PRODUCTION_COUNTRY.HONKONG;
                case "Indie":
                    return PRODUCTION_COUNTRY.INDIE;
                case "Meksyk":
                    return PRODUCTION_COUNTRY.MEKSYK;
                case "RFN":
                    return PRODUCTION_COUNTRY.RFN;
                case "ZSRR":
                    return PRODUCTION_COUNTRY.ZSRR;
                case "Niemcy":
                    return PRODUCTION_COUNTRY.NIEMCY;
                case "Japonia":
                    return PRODUCTION_COUNTRY.JAPONIA;
                dafault:
                    return PRODUCTION_COUNTRY.UNKNOWN;
            };

            return PRODUCTION_COUNTRY.UNKNOWN;
        }
    }
}
