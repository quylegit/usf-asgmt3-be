﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usf_asgmt3_api.Integration;
using usf_asgmt3_api.Integration.IxTrading;
using usf_asgmt3_api.Integration.LocalDataRepo;
using usf_asgmt3_api.model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace usf_asgmt3_api.Controllers
{

    //[Route("api/[controller]")]
    public class StockController : Controller
    {
        public LocalDBContext dbContext;
        private readonly IIxTradingRepo iIxTradingRepo;

        public StockController(LocalDBContext dbContext)
        {
            this.dbContext = dbContext;
            this.iIxTradingRepo = new IxTradingRepo();
        }


        [HttpPut("/stock/syncSymbols/")]
        public List<Symbol> SyncSymbols()
        {

            var repo = new IxTradingRepo();
            var list = repo.GetSymbolsAsync().Result;
            int i = 0;

            foreach (var item in list)
            {
                if (item.symbol != null)
                {
                    var exist = dbContext.Symbols.Any(a => a.symbol == item.symbol);

                    if (!exist)
                    {
                        try
                        {
                            dbContext.Symbols.Add(item);
                        }
                        catch (Exception ex)
                        {
                            dbContext.Entry(item).State = EntityState.Detached;
                        }
                    }
                }
                i++;

                if (i % 100 == 0)
                    dbContext.SaveChanges(); // lets save this hundred batch
            }

            dbContext.SaveChanges(); // get the last un saved records

            return list;

        }


        [HttpPut("/stock/syncSymbolPrices/")]
        public bool SyncSymbolPrices(List<string> symbols)
        {

            var intrinio = new IntrinioRepo();
            //var list = new List<string> { symbol };

            DateTime now = DateTime.Now.Date;
            DateTime start = now.AddYears(-1);
            string frequency = "daily";
            foreach (var symbol in symbols)
            {
                var cleanedSymbol = symbol?.ToUpper();

                var list = intrinio.GetCompanyPriceAsync(cleanedSymbol, frequency, start, now).Result;

                // clean old data
                //dbContext.Prices.FromSql($"Delete from dbo.Prices where ticket = '{cleanedSymbol}'");

                //todo add repo for db
                foreach (var item in list)
                {
                    item.ticker = cleanedSymbol;
                    dbContext.Prices.Add(item);
                }
                dbContext.SaveChanges();
            }
            return true;
        }

        [HttpPut("/stock/syncCompanyDetails/")]
        public bool SyncCompanyDetails(List<string> symbols)
        {
            var list = iIxTradingRepo.GetCompanyDetailAsync(symbols).Result;

            foreach (var item in list)
            {
                //clean old data
                //dbContext.Company_Details.FromSql($"Delete from dbo.Company_Details where symbol = '{item.symbol}'");
                dbContext.Company_Details.Add(item);
            }
            dbContext.SaveChanges();

            return true;
        }

        [HttpPut("/stock/syncCompanyDividends/")]
        public bool SyncCompanyDividends(List<string> symbols)
        {            
            var list = iIxTradingRepo.GetCompanyDividendAsync(symbols).Result;

            foreach (var item in list)
            {
                //clean old data
                //dbContext.Company_Dividends.FromSql($"Delete from dbo.Company_Dividends where symbol = '{item.symbol}'");
                dbContext.Company_Dividends.Add(item);
            }
            dbContext.SaveChanges();

            return true;
        }

        [HttpPut("/stock/syncCompanyFinalcials/")]
        public bool SyncCompanyFinalcials(List<string> symbols)
        {
            var list = iIxTradingRepo.GetCompanyFinancialAsync(symbols).Result;

            foreach (var item in list)
            {
                ////clean old data
                //dbContext.Company_Financials.FromSql($"Delete from dbo.Company_Financials where symbol = '{item.symbol}'");
                dbContext.Company_Financials.Add(item);
            }
            dbContext.SaveChanges();

            return true;
        }


        [HttpGet("/stock/getStocks/")]
        public List<Symbol> GetStocks()
        {
            var list = dbContext.Symbols.ToList();
            return list;
        }

        // GET api/values/5
        [HttpGet("/stock/getStockDividend/{symbol}")]
        public List<Company_Dividend> GetStockDividend(string symbol)
        {
            return dbContext.Company_Dividends.Where(a => a.symbol.ToLower() == symbol.ToLower()).ToList();
        }

        [HttpGet("/stock/getStockFinance/{symbol}")]
        public List<Company_Financial> GetStockFinance(string symbol)
        {
            return dbContext.Company_Financials.Where(a => a.symbol.ToLower() == symbol.ToLower()).ToList();
        }
    }
}