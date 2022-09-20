using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VenturaHR.Helpers
{
    public class DropDownListHelper
    {
        private class Item
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public Item(string text, string value)
            {
                this.Text = text;
                this.Value = value;
            }
        }
        private IEnumerable<SelectListItem> GetSelectListItem(List<Item> items, string value)
        {
            foreach (var item in items)
                yield return new SelectListItem(item.Text, item.Value, !string.IsNullOrWhiteSpace(value) ? value.ToUpper() == item.Value.ToUpper() : false);
        }
        public List<SelectListItem> GetYesOrNo(string value)
        {
            return GetSelectListItem(new List<Item>() { new Item("Sim", "True"), new Item("Não", "False") }, value).ToList();
        }
        public List<SelectListItem> GetGenders(string value)
        {
            return GetSelectListItem(new List<Item>() { new Item("Masculino", "m"), new Item("Feminino", "f") }, value).ToList();
        }
        public List<SelectListItem> GetStates(string value)
        {
            return GetSelectListItem(new List<Item>() {
                new Item("Acre"               , "AC"),
                new Item("Alagoas"            , "AL"),
                new Item("Amapá"              , "AP"),
                new Item("Amazonas"           , "AM"),
                new Item("Bahia"              , "BA"),
                new Item("Ceará"              , "CE"),
                new Item("Distrito Federal"   , "DF"),
                new Item("Espirito Santo"     , "ES"),
                new Item("Goiás"              , "GO"),
                new Item("Maranhão"           , "MA"),
                new Item("Mato Grosso do Sul" , "MS"),
                new Item("Mato Grosso"        , "MT"),
                new Item("Minas Gerais"       , "MG"),
                new Item("Pará"               , "PA"),
                new Item("Paraíba"            , "PB"),
                new Item("Paraná"             , "PR"),
                new Item("Pernambuco"         , "PE"),
                new Item("Piauí"              , "PI"),
                new Item("Rio de Janeiro"     , "RJ"),
                new Item("Rio Grande do Norte", "RN"),
                new Item("Rio Grande do Sul"  , "RS"),
                new Item("Rondônia"           , "RO"),
                new Item("Roraima"            , "RR"),
                new Item("Santa Catarina"     , "SC"),
                new Item("São Paulo"          , "SP"),
                new Item("Sergipe"            , "SE"),
                new Item("Tocantins"          , "TO")
            }, value).ToList();
        }
        public IEnumerable<SelectListItem> GetUfs(string value)
        {
            var ufs = GetStates(value);
            ufs.ForEach(x => x.Text = x.Value);

            return ufs;
        }

        public IEnumerable<SelectListItem> ToSelectListItem<T, TKey, TValue>(IEnumerable<T> data, Func<T, TKey> key, Func<T, TValue> item, string value = "")
        {
            var items = data.Select(x => new Item(item.Invoke(x)?.ToString(), key.Invoke(x)?.ToString())).ToList();

            return GetSelectListItem(items, value);
        }
    }
}
