using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PSE.Decoder.Database;
using PSE.Model.Events;
using PSE.Model.Common;
using PSE.Model.Output.Models;

namespace PSE.Decoder
{

    public class Decoder : IDisposable, IDecoder
    {

        private const string NOT_FOUND = "[NOT_FOUND]";

        private readonly ServiceProvider? _serviceProvider;        

        public event ExternalCodifyErrorOccurredEventHandler? ExternalCodifyErrorOccurred;

        public Decoder(AppSettings appSettings)
        {
            _serviceProvider = null;
            if (appSettings.DecoderEnabled)
            {
                ServiceCollection _serviceCollection = new ServiceCollection();
                _serviceCollection.AddDbContext<BOSSDbContext>(_options =>
                {
                    _options.UseSqlServer(appSettings.DecoderStringConnection);                       
                    _options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            }
        }

        public virtual bool Decode(ExternalCodifyRequestEventArgs e)
        {
            bool _decoded = false;
            try
            {
                if (_serviceProvider != null && e != null && string.IsNullOrEmpty(e.PropertyKey) == false)
                {
                    e.PropertyValue = NOT_FOUND;
                    BOSSDbContext? _context = _serviceProvider.GetService<BOSSDbContext>();
                    if (_context != null)
                    {                        
                        if (e.SectionName == nameof(Section1) && e.PropertyName == nameof(AssetStatement.Advisor))
                        {
                            if (_context.AdaAuIde.Any(_flt => _flt.AuiNumPer == e.PropertyKey))
                                e.PropertyValue = _context.AdaAuIde.First(_flt => _flt.AuiNumPer == e.PropertyKey).AuiNome;
                        }
                        else if (e.SectionName == nameof(Section3) && e.PropertyName == nameof(KeyInformation.Portfolio))
                        {
                            if (e.PropertyKey.Length > 0 && _context.Tabelle.Any(_flt => _flt.Tab == "N121" && _flt.Code == e.PropertyKey.Substring(0, 1)))
                            {
                                e.PropertyValue = _context.Tabelle.First(_flt => _flt.Tab == "N121" && _flt.Code == e.PropertyKey.Substring(0, 1)).TextI;
                                e.PropertyValue = e.PropertyKey[1..] + " - " + e.PropertyValue;
                            }
                        }
                        else if (e.SectionName == nameof(Section3) && e.PropertyName == nameof(KeyInformation.Service))
                        {
                            if (_context.Tabelle.Any(_flt => _flt.Tab == "L066" && _flt.Code == e.PropertyKey))
                                e.PropertyValue = _context.Tabelle.First(_flt => _flt.Tab == "L066" && _flt.Code == e.PropertyKey).TextI;
                        }
                        if (!(string.IsNullOrEmpty(e.PropertyValue) || string.IsNullOrWhiteSpace(e.PropertyValue) || e.PropertyValue == NOT_FOUND))
                        {
                            e.PropertyValue = e.PropertyValue.Trim();
                            _decoded = true;
                        }
                        else if (string.IsNullOrEmpty(e.PropertyValue) == false && e.PropertyValue == NOT_FOUND)
                        {
                            e.ErrorOccurred = "Element having the specified property key has not been found into target db!";
                            e.Cancel = true;
                            this.ExternalCodifyErrorOccurred?.Invoke(this, e);
                        }
                    }
                }
                else if (_serviceProvider == null && e != null && string.IsNullOrEmpty(e.PropertyKey) == false) // feature not activated
                    e.PropertyValue = e.PropertyKey;
            }
            catch(Exception _ex)
            {
                throw new Exception(_ex.Message);
            }
            return _decoded;
        }

        public void Dispose()
        {
            if (_serviceProvider != null)
                _serviceProvider.Dispose();
        }

    }

}
