using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Library.TaskManagement
{
    public class ListNavigator<T> : DependencyObject
    {

        private int currentPage;
        private List<T> state;
        private ObservableCollection<T> window;

        private int pageSize;
        private int lastPage
        {
            get
            {
                var val = state.Count / pageSize;

                if (state.Count % pageSize > 0)
                {
                    //if there is a partial page at the end, that is the actual last page.
                    val++;
                }

                return val;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return currentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return currentPage < lastPage;
            }

        }
        public ListNavigator(List<T> list, ObservableCollection<T> window, int pageSize = 5)
        {
            this.pageSize = pageSize;
            this.currentPage = 1;
            state = list;
            this.window = window;
        }

        public ListNavigator(NamedList<T> namedList, ObservableCollection<T> window, int pageSize = 5)
        {
            this.pageSize = pageSize;
            this.currentPage = 1;
            state = namedList.list;
            this.window = window;
        }

        public ObservableCollection<T> GoForward()
        {
            if (currentPage + 1 > lastPage)
            {
                throw new PageFaultException("Cannot navigate to the right of the last page in the list!");
            }
            currentPage++;
            return GetWindow();
        }

        public ObservableCollection<T> GoBackward()
        {
            if (currentPage - 1 <= 0)
            {
                throw new PageFaultException("Cannot navigate to the left of the first page in the list!");
            }
            currentPage--;
            return GetWindow();
        }

        public ObservableCollection<T> GoToPage(int page)
        {
            if (page <= 0 || page > lastPage)
            {
                throw new PageFaultException("Cannot navigate to a page outside of the bounds of the list!");
            }
            currentPage = page;
            return GetWindow();
        }

        public ObservableCollection<T> GetCurrentPage()
        {
            if (lastPage == 0)
            {
                return new ObservableCollection<T>();
            }
            return GoToPage(currentPage);
        }

        public ObservableCollection<T> GoToFirstPage()
        {
            currentPage = 1;
            return GetWindow();
        }

        public ObservableCollection<T> GoToLastPage()
        {
            currentPage = lastPage;
            return GetWindow();
        }

        private ObservableCollection<T> GetWindow()
        {//(currentPage*pageSize) + pageSize
            window.Clear();
            for (int i = (currentPage - 1) * pageSize; i < (currentPage - 1) * pageSize + pageSize && i < state.Count; i++)
            {
                window.Add(state[i]);
            }

            return window;
        }




    }



    public class PageFaultException : Exception
    {
        public PageFaultException(string message) : base(message)
        {

        }
    }
}
