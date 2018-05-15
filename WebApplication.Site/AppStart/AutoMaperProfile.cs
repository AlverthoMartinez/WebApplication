
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using WebApplication.Site.Helper;
using WebApplication.Domain;
using WebApplication.Site.Models.DTO;
using WebApplication.Data;
// using WebApplication.Site.App_Start;

namespace WebApplication.Site.AppStart
{
    public class AutoMaperProfile : Profile
    {
        public AutoMaperProfile()
        {
            CreateMap<Todo, TodoDTO>()
                .ReverseMap();
        }


        private void CreateStringToEnumMap<E>()
            where E : struct, IConvertible // Enum
        {
            if (!typeof(E).IsEnum)
            {
                throw new ArgumentException("Generic parameter 'Enum' must be an enumerated type");
            }

            this.CreateMap<string, E>().ConvertUsing(this.ConvertStringToEnum<E>);
            this.CreateMap<E, string>().ConvertUsing(EnumHelper.DisplayName<E>);
        }

        private E ConvertStringToEnum<E>(string value)
            where E : struct, IConvertible // Enum
        {
            E e;
            Enum.TryParse<E>(value, out e);

            return e;
        }
    }

    //public class StringToEnumConverter<Enum> : ITypeConverter<string, Enum>
    //    where Enum : struct, IConvertible // Enum
    //{
    //    public Enum Convert(ResolutionContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    // public static class ListExtension
    // {
    //     public static IEnumerable<T> DoAfter<T>(this IEnumerable<T> list, Action<T> todo)
    //     {
    //         if (list != null)
    //         {
    //             list.Each(todo);
    //         }
    //         return list;
    //     }
    // }

    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapMembers<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> obj,
            Expression<Func<TDestination, object>> destinationMember,
            Expression<Func<TSource, object>> sourceMember)
        {
            return obj.ForMember(destinationMember, opt => opt.MapFrom(sourceMember));
        }
    }

    public static class AppMapper
    {
        public static IEnumerable<TDestination> MapList<TDestination>(object source)
        {
            return AutoMapper.Mapper.Map<IEnumerable<TDestination>>(source);
        }

        public static TDestination Map<TDestination>(object source)
        {
            return AutoMapper.Mapper.Map<TDestination>(source);
        }
    }
}
